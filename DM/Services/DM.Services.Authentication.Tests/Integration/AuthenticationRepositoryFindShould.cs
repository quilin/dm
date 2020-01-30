using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Tests.Core;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;
using DtoSession = DM.Services.Authentication.Dto.Session;

namespace DM.Services.Authentication.Tests.Integration
{
    public class AuthenticationRepositoryFindShould : DbTestBase
    {
        [Fact]
        public async Task FindUserByIdentifier()
        {
            var userId = Guid.NewGuid();
            using var rdb = GetRdb(nameof(FindUserByIdentifier));
            rdb.Users.Add(new User
            {
                UserId = userId,
                Login = "MyUser",
                Email = "my@user.mail"
            });
            await rdb.SaveChangesAsync();

            var authenticationRepository = new AuthenticationRepository(rdb, null, GetMapper());
            (await authenticationRepository.FindUser(userId)).Should().NotBeNull();
            (await authenticationRepository.FindUser(Guid.NewGuid())).Should().BeNull();
        }

        [Fact]
        public async Task FindUserSession()
        {
            var sessionId = Guid.NewGuid();
            using var mdb = GetMongoClient(nameof(FindUserSession));
            await mdb.Client.GetCollection<UserSessions>()
                .InsertManyAsync(new[]
                {
                    new UserSessions
                    {
                        Id = Guid.NewGuid(),
                        Sessions = new List<Session>
                        {
                            new Session
                            {
                                Id = sessionId,
                                ExpirationDate = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                IsPersistent = false
                            },
                            new Session
                            {
                                Id = Guid.NewGuid(),
                                ExpirationDate = new DateTime(2019, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                                IsPersistent = true
                            }
                        }
                    },
                    new UserSessions
                    {
                        Id = Guid.NewGuid(),
                        Sessions = new List<Session>
                        {
                            new Session
                            {
                                Id = Guid.NewGuid(),
                                ExpirationDate = DateTime.Today,
                                IsPersistent = false
                            }
                        }
                    }
                });

            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            var actual = await authenticationRepository.FindUserSession(sessionId);
            actual.Should().BeEquivalentTo(new DtoSession
            {
                Id = sessionId,
                ExpirationDate = new DateTimeOffset(new DateTime(2020, 1, 1), TimeSpan.Zero),
                IsPersistent = false
            });
            (await authenticationRepository.FindUserSession(Guid.NewGuid())).Should().BeNull();
        }

        [Fact]
        public async Task FindUserSettings()
        {
            var userId = Guid.NewGuid();
            using var mdb = GetMongoClient(nameof(FindUserSettings));
            await mdb.Client.GetCollection<UserSettings>()
                .InsertManyAsync(new[]
                {
                    new UserSettings
                    {
                        Id = userId,
                        ColorSchema = ColorSchema.Night,
                        Paging = new PagingSettings
                        {
                            CommentsPerPage = 23,
                            MessagesPerPage = 12,
                            PostsPerPage = 10,
                            TopicsPerPage = 55
                        },
                        NannyGreetingsMessage = "Hi, I'm your nanny!",
                    }
                });

            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            (await authenticationRepository.FindUserSettings(userId)).Should().BeEquivalentTo(new Dto.UserSettings
            {
                Id = userId,
                ColorSchema = ColorSchema.Night,
                NannyGreetingsMessage = "Hi, I'm your nanny!",
                Paging = new Dto.PagingSettings
                {
                    CommentsPerPage = 23,
                    MessagesPerPage = 12,
                    PostsPerPage = 10,
                    TopicsPerPage = 55
                }
            });
            (await authenticationRepository.FindUserSettings(Guid.NewGuid())).Should()
                .BeEquivalentTo(Dto.UserSettings.Default);
        }

        [Fact]
        public async Task AddSessionAsUpsert()
        {
            using var mdb = GetMongoClient(nameof(AddSessionAsUpsert));
            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();

            await authenticationRepository.AddSession(userId, new Session
            {
                Id = sessionId,
                IsPersistent = true,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 1), DateTimeKind.Utc)
            });

            var sessions = await mdb.Client.GetCollection<UserSessions>()
                .Find(FilterDefinition<UserSessions>.Empty)
                .ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].Should().BeEquivalentTo(new UserSessions
            {
                Id = userId,
                Sessions = new List<Session>
                {
                    new Session
                    {
                        Id = sessionId,
                        ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 1), DateTimeKind.Utc),
                        IsPersistent = true
                    }
                }
            });

            await authenticationRepository.AddSession(userId, new Session
            {
                Id = Guid.NewGuid(),
                IsPersistent = false,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 2), DateTimeKind.Utc)
            });

            sessions = await mdb.Client.GetCollection<UserSessions>()
                .Find(FilterDefinition<UserSessions>.Empty)
                .ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].Sessions.Should().HaveCount(2);
        }

        [Fact]
        public async Task RemoveSession()
        {
            using var mdb = GetMongoClient(nameof(RemoveSession));
            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            var userId = Guid.NewGuid();
            var sessionId1 = Guid.NewGuid();
            var sessionId2 = Guid.NewGuid();

            await authenticationRepository.AddSession(userId, new Session
            {
                Id = sessionId1,
                IsPersistent = true,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 1), DateTimeKind.Utc)
            });
            await authenticationRepository.AddSession(userId, new Session
            {
                Id = sessionId2,
                IsPersistent = false,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 2), DateTimeKind.Utc)
            });

            await authenticationRepository.RemoveSession(userId, sessionId1);

            var sessions = await mdb.Client.GetCollection<UserSessions>()
                .Find(FilterDefinition<UserSessions>.Empty)
                .ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].Sessions.Should().HaveCount(1);
        }

        [Fact]
        public async Task RemoveSessions()
        {
            using var mdb = GetMongoClient(nameof(RemoveSessions));
            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            var userId = Guid.NewGuid();

            await authenticationRepository.AddSession(userId, new Session
            {
                Id = Guid.NewGuid(),
                IsPersistent = true,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 1), DateTimeKind.Utc)
            });
            await authenticationRepository.AddSession(userId, new Session
            {
                Id = Guid.NewGuid(),
                IsPersistent = false,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 2), DateTimeKind.Utc)
            });

            await authenticationRepository.RemoveSessions(userId);

            var sessions = await mdb.Client.GetCollection<UserSessions>()
                .Find(FilterDefinition<UserSessions>.Empty)
                .ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].Sessions.Should().HaveCount(0);
        }

        [Fact]
        public async Task RefreshSessions()
        {
            using var mdb = GetMongoClient(nameof(RefreshSessions));
            var authenticationRepository = new AuthenticationRepository(null, mdb.Client, GetMapper());
            var userId = Guid.NewGuid();
            var sessionId1 = Guid.NewGuid();
            var sessionId2 = Guid.NewGuid();

            await authenticationRepository.AddSession(userId, new Session
            {
                Id = sessionId1,
                IsPersistent = true,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 1), DateTimeKind.Utc)
            });
            await authenticationRepository.AddSession(userId, new Session
            {
                Id = sessionId2,
                IsPersistent = false,
                ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 2), DateTimeKind.Utc)
            });

            await authenticationRepository.RefreshSession(userId, sessionId1,
                new DateTimeOffset(new DateTime(2019, 6, 14, 11, 0, 0), TimeSpan.Zero));

            var sessions = await mdb.Client.GetCollection<UserSessions>()
                .Find(FilterDefinition<UserSessions>.Empty)
                .ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].Sessions.Should().BeEquivalentTo(new List<Session>
            {
                new Session
                {
                    Id = sessionId1,
                    IsPersistent = true,
                    ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 6, 14, 11, 0, 0), DateTimeKind.Utc)
                },
                new Session
                {
                    Id = sessionId2,
                    IsPersistent = false,
                    ExpirationDate = DateTime.SpecifyKind(new DateTime(2019, 1, 2), DateTimeKind.Utc)
                }
            });
        }
    }
}
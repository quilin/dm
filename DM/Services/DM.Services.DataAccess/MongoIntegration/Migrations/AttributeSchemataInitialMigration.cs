using System;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration.Migrations;

/// <inheritdoc />
public class AttributeSchemataInitialMigration : DatabaseMigration
{
    private static readonly Guid SchemaD20Id = Guid.Parse("0982eb99-54fc-4602-ad0a-b21b404e0d4b");
    private static readonly Guid SchemaSimpleId = Guid.Parse("a87ed42e-464b-4db1-b62b-5f8ada7cce85");

    /// <inheritdoc />
    public AttributeSchemataInitialMigration() : base("1.0.0")
    {
    }

    /// <inheritdoc />
    public override void Up(IMongoDatabase db)
    {
        var d20ConstraintsProvider = () => new NumberAttributeConstraints
        {
            Required = true,
            MinValue = 0,
            MaxValue = 40
        };
        var simpleConstraintsProvider = () => new ListAttributeConstraints
        {
            Required = true,
            Values = new ListAttributeValue[]
            {
                new() { Value = "Ужасно", Modifier = -30 },
                new() { Value = "Очень плохо", Modifier = -20 },
                new() { Value = "Плохо", Modifier = -10 },
                new() { Value = "Средне", Modifier = 0 },
                new() { Value = "Хорошо", Modifier = 10 },
                new() { Value = "Очень хорошо", Modifier = 20 },
                new() { Value = "Великолепно", Modifier = 30 }
            }
        };
        var attributeTitles = new[] { "Сила", "Ловкость", "Телосложение", "Интеллект", "Мудрость", "Харизма" };

        db.GetCollection<AttributeSchema>("AttributeSchemata")
            .InsertMany(new[]
            {
                new AttributeSchema
                {
                    Id = SchemaD20Id,
                    Type = SchemaType.Public,
                    Title = "D20",
                    Specifications = attributeTitles.Select(title => new AttributeSpecification
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                        Constraints = d20ConstraintsProvider.Invoke()
                    }),
                    IsRemoved = false,
                    UserId = null
                },
                new AttributeSchema
                {
                    Id = SchemaSimpleId,
                    Type = SchemaType.Public,
                    Title = "Хорошо-Средне-Плохо",
                    Specifications = attributeTitles.Select(title => new AttributeSpecification
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                        Constraints = simpleConstraintsProvider.Invoke()
                    }),
                    IsRemoved = false,
                    UserId = null
                }
            });
    }

    /// <inheritdoc />
    public override void Down(IMongoDatabase db)
    {
        db.GetCollection<AttributeSchema>("AttributeSchemata")
            .DeleteMany(schema => schema.Id == SchemaD20Id || schema.Id == SchemaSimpleId);
    }
}
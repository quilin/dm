import {User, UserRole} from '@/api/models/community';

export function userIsAdmin(user: User | null): boolean {
  return user !== null && user.roles.some(r => r === UserRole.Administrator);
}

export function userIsHighAuthority(user: User | null): boolean {
  return user !== null && user.roles.some(r =>
    r === UserRole.Administrator ||
    r === UserRole.SeniorModerator);
}

export function userIsAuthority(user: User | null): boolean {
  return user !== null && user.roles.some(r =>
    r === UserRole.Administrator ||
    r === UserRole.SeniorModerator ||
    r === UserRole.RegularModerator);
}

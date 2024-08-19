import { UserRole } from "@/api/models/community";
import type { User } from "@/api/models/community";

export function userIsAdmin(user: User | null): boolean {
  return user !== null && user.roles.some((r) => r === UserRole.Administrator);
}

export function userIsHighAuthority(user: User | null): boolean {
  return (
    user !== null &&
    user.roles.some(
      (r) => r === UserRole.Administrator || r === UserRole.SeniorModerator,
    )
  );
}

export function userIsAuthority(user: User | null): boolean {
  return (
    user !== null &&
    user.roles.some(
      (r) =>
        r === UserRole.Administrator ||
        r === UserRole.SeniorModerator ||
        r === UserRole.RegularModerator,
    )
  );
}

export function userIsNanny(user: User | null): boolean {
  return user !== null && user.roles.some((r) => r === UserRole.NannyModerator);
}

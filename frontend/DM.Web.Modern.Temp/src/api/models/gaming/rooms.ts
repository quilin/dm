import type { Id, Served } from "@/api/models";

export type RoomId = Id<string>;
export type Room = {
  id: Served<RoomId>;
  title: string;
};

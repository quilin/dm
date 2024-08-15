export enum ColorSchema {
  Modern = "Modern",
  Pale = "Pale",
  Classic = "Classic",
  ClassicPale = "ClassicPale",
  Night = "Night",
}

export type PagingSettings = {
  postsPerPage: number;
  commentsPerPage: number;
  topicsPerPage: number;
  messagesPerPage: number;
  entitiesPerPage: number;
};

export type UserSettings = {
  nannyGreetingsMessage: string;
  colorSchema: ColorSchema;
  paging: PagingSettings;
};

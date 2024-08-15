export type RegisterCredentials = {
  email: string;
  login: string;
  password: string;
};

export type LoginCredentials = {
  login: string;
  password: string;
  rememberMe: boolean;
};

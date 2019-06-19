export interface LoginCredentials {
  login: string;
  password: string;
  rememberMe: boolean;
}

export interface Rating {
  enabled: boolean;
  quality: number;
  quantity: number;
}

export interface User {
  login: string;
  roles: string[];
  profilePictureUrl: string;
  rating: Rating;
  online: string;
}

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
  status: string;
  name: string;
  location: string;
}

export interface Poll {
  id: string;
  ends: string;
  title: string;
  options: PollOption[];
}

export interface PollOption {
  id: string;
  text: string;
  votesCount: number;
  voted: boolean | null;
}

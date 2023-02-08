import { User } from '@/api/models/community';

export * from './games';
export * from './attributes';
export * from './characters';
export * from './rooms';

export interface Comment {
    id: string;
    author: User;
    created: string;
    updated: string | null;
    text: string;
    likes: User[];
}

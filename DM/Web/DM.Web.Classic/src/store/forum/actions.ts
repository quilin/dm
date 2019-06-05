import { ActionTree } from 'vuex';
import forumApi from '@/api/requests/forumApi';
import ForumState from './forumState';
import RootState from './../rootState';

const actions: ActionTree<ForumState, RootState> = {
  fetchFora({ commit }): Promise<void> {
    
  }
}

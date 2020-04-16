import { MutationTree } from 'vuex';
import LayoutState from './layoutState';
import RootState from './../rootState';

const mutations: MutationTree<LayoutState> = {
    updateMenuBar(state, status: boolean) {
        state.menuBar = status;
    },
}

export default mutations;

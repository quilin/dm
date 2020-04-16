import { GetterTree } from 'vuex';
import LayoutState from './layoutState';
import RootState from './../rootState';

const getters: GetterTree<LayoutState, RootState> = {
    menuBarStatus(state): boolean {
        return state.menuBar;
    },
}

export default getters;

import { ActionTree } from 'vuex';
import RootState from "./../rootState";
import LayoutState from "./layoutState";
import getters from "./getters";


const actions: ActionTree<LayoutState, RootState> = {
    showMenuBar({ commit }): void {
        commit('updateMenuBar', true);
    },
    hideMenuBar({ commit }): void {
        commit('updateMenuBar', false);
    },
    toggleMenuBar({ commit, state }): void {
        const toggle = !state.menuBar;
        commit('updateMenuBar', toggle);
    },
}

export default actions;

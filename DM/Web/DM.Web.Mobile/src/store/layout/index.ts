import { Module, GetterTree, MutationTree } from 'vuex';
import LayoutState from "./layoutState";
import getters from "./getters";
import mutations from "./mutations";
import actions from "./actions";
import RootState from './../rootState';

const state: LayoutState = {
    menuBar: false,
}

const layout: Module<LayoutState, RootState> = {
    state,
    getters,
    mutations,
    actions,
}

export default layout;

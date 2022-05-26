import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import {register} from '../api/authorization'

export const authSlice= createSlice({
  name: 'counter',
  initialState:{
    www:"abebe",
   token:"",
   username:"",
   data:[]
  },
  reducers: {
    
    loginAction: (state,action) => {
       state.token=action.payload
    },
    registration:(state,action)=>{
      state.username = action.payload
    },
    setData:(state,action)=>{
        state.data=action.payload
    }
}
})

// Action creators are generated for each case reducer function
export const {  loginAction,registration,setData } = authSlice.actions

export default authSlice.reducer
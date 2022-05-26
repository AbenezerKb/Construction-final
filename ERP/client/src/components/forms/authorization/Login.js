import React, { useState } from "react"
import axios from "axios"
import { Link, useNavigate } from "react-router-dom"
import { FcFlowChart } from "react-icons/fc"
import api from "../../../api/api"
import { ToastContainer, toast } from "react-toastify"
import "react-toastify/dist/ReactToastify.css"
import { useAuth, useAuthUpdate } from "../../../contexts/AuthContext"

const Login = () => {
  const navigate = useNavigate()
  const contextData = useAuth()
  const updateAuthContext = useAuthUpdate()
  const [value, setValue] = useState({
    username: "",
    password: "",
  })

  const toastOption = {
    position: "bottom-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    theme: "dark",
  }

  const handleLogin = async (e) => {
    e.preventDefault()
    if (!value.username) {
      toast.error("Please provide your username", toastOption)
    } else if (!value.password) {
      toast.error("Please provide your password", toastOption)
    } else {
      try {
        const res = await api.post("Authorization/login", value)
        updateAuthContext({
          ...contextData,
          token: res.data.token,
          data: res.data,
        })
        localStorage.setItem("token", res.data.token)
        localStorage.setItem("data", JSON.stringify(res.data))

        axios.defaults.headers.common["Authorization"] =
          "bearer " + res.data.token

        navigate("/")
      } catch (err) {
        toast.error("incorrect username or password", toastOption)
      }
    }
  }

  return (
    <div className="loginAndRegistration">
      {contextData?.username && (
        <div style={{ maxWidth: "600px", margin: "auto" }}>
          <div
            style={{ marginTop: "30px" }}
            className="alert alert-success"
            role="alert"
          >
            You have successfully registered. Use username:{" "}
            {contextData.username} to login.
          </div>
        </div>
      )}

      <div>
        <form className="login" style={{ backgroundColor: "#f1f1f1" }}>
          <h3 style={{ color: "grey", paddingTop: "10px" }}>
            <FcFlowChart
              style={{
                width: "40px",
                height: "40px",
                marginBottom: "10px",
              }}
            />{" "}
            ERP
          </h3>
          <h3>Login</h3>
          <div className="form-group">
            <label>Username</label>
            <input
              onChange={(e) => setValue({ ...value, username: e.target.value })}
              value={value.username}
              type="username"
              className="form-control"
              placeholder="Enter username"
            />
          </div>
          <div className="form-group">
            <label>Password</label>
            <input
              onChange={(e) => setValue({ ...value, password: e.target.value })}
              value={value.password}
              type="password"
              className="form-control"
              placeholder="Enter password"
            />
          </div>
          <div className="form-group"></div>
          <button
            onClick={handleLogin}
            type="submit"
            className="btn btn-primary btn-block"
          >
            Login
          </button>
          <span className="signup__link">
            Not registered? <Link to={"/registration"}>Create account</Link>
          </span>
        </form>
      </div>
      <ToastContainer />
    </div>
  )
}

export default Login

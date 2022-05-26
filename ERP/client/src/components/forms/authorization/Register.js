import React, { useEffect, useState } from "react"
import "./auth.css"
import api from "../../../api/api"
import { FcFlowChart } from "react-icons/fc"
import { ToastContainer, toast } from "react-toastify"
import "react-toastify/dist/ReactToastify.css"
import { Link, useNavigate } from "react-router-dom"
import { useAuth, useAuthUpdate } from "../../../contexts/AuthContext"

const SignUp = () => {
  const navigate = useNavigate()

  const contextData = useAuth()
  const updateAuthContext = useAuthUpdate()
  const [allUserRole, setAllUserRole] = useState([])
  const [allSite, setAllSite] = useState([])

  const [passwordConfirm, setPasswordConfirm] = useState("")
  const [value, setValue] = useState({
    fName: "",
    mName: "",
    lName: "",
    position: "",
    employeeSiteId: "",
    userRoleId: 0,
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

  useEffect(() => {
    async function fetchRole() {
      const res = await api.get("/UserRole")
      setAllUserRole(res.data)
    }
    fetchRole()
    async function fetchSite() {
      const res = await api.get("/site")
      setAllSite(res.data)
    }
    fetchSite()
  }, [])
  const handleRegister = async (e) => {
    e.preventDefault()
    const {
      employeeSiteId,
      fName,
      lName,
      mName,
      password,
      userRoleId,
      position,
    } = value
    if (!fName) {
      toast.error("Please provide your First name", toastOption)
    } else if (!lName) {
      toast.error("Please provide your Last name", toastOption)
    } else if (!mName) {
      toast.error("Please provide your Middle name", toastOption)
    } else if (!position) {
      toast.error("Please provide your Postion", toastOption)
    } else if (!employeeSiteId) {
      toast.error("Please provide your Site", toastOption)
    } else if (!userRoleId) {
      toast.error("Please provide your User Role", toastOption)
    } else if (password !== passwordConfirm) {
      toast.error(
        "Your password and confirm password is not match",
        toastOption
      )
    } else if (password.length < 8) {
      toast.error("The password must be greater than 8 characters", toastOption)
    } else {
      try {
        const res = await api.post("Authorization/register", value)
        toast.success(
          "Registration successFull! please wait for approval.",
          toastOption
        )
        updateAuthContext({ ...contextData, username: res.data.username })
        navigate("/login")
      } catch (err) {
        toast.error("Wrong input please try again", toastOption)
      }
    }
  }

  return (
    <div className="loginAndRegistration">
      <form
        className="signup"
        autoComplete="off"
        style={{ backgroundColor: "#f1f1f1" }}
      >
        <h3 style={{ color: "grey", paddingTop: "10px" }}>
          <FcFlowChart
            style={{ width: "40px", height: "40px", marginBottom: "10px" }}
          />{" "}
          ERP
        </h3>
        <h3>Sign Up</h3>
        <div className="signup__row">
          <div>
            <div className="form-group">
              <label>First name</label>
              <input
                name="fName"
                onChange={(e) => setValue({ ...value, fName: e.target.value })}
                value={value.fName}
                type="text"
                className="form-control"
                placeholder="First name"
              />
            </div>
            <div className="form-group">
              <label>Last name</label>
              <input
                name="lName"
                onChange={(e) => setValue({ ...value, lName: e.target.value })}
                value={value.lName}
                type="text"
                className="form-control"
                placeholder="Last name"
              />
            </div>
            <div className="form-group">
              <label>Position</label>
              <input
                name="position"
                onChange={(e) =>
                  setValue({ ...value, position: e.target.value })
                }
                autoComplete="false"
                value={value.position}
                type="text"
                className="form-control"
                placeholder="Position"
              />
            </div>
            <div className="form-group">
              <label>Password</label>
              <input
                name="password"
                onChange={(e) =>
                  setValue({ ...value, password: e.target.value })
                }
                autoComplete="false"
                value={value.password}
                type="password"
                className="form-control"
                placeholder="Password"
              />
            </div>
          </div>
          <div>
            <div className="form-group">
              <label>Middle Name</label>
              <input
                name="mName"
                onChange={(e) => setValue({ ...value, mName: e.target.value })}
                value={value.mName}
                type="text"
                className="form-control"
                placeholder="Middle name"
              />
            </div>

            <div className="form-group">
              <label htmlFor="employeeSiteId">Site</label>
              <br />

              <select
                onChange={(e) =>
                  setValue({ ...value, employeeSiteId: e.target.value })
                }
                value={value.employeeSiteId}
                className="form-control"
                name="employeeSiteId"
                id="employeeSiteId"
              >
                <option hidden>Select your site</option>

                {allSite.map((val, i) => (
                  <option key={i} value={val.siteId}>
                    {val.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="userRoleId">User Role</label>
              <br />

              <select
                onChange={(e) =>
                  setValue({ ...value, userRoleId: e.target.value })
                }
                value={value.userRoleId}
                className="form-control"
                name="userRoleId"
                id="userRoleId"
              >
                <option hidden>Select your role</option>
                {allUserRole.map((val, i) => (
                  <option key={i} value={val.roleId}>
                    {val.role}
                  </option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label>Confirm Password</label>
              <input
                name="passwordConfirm"
                onChange={(e) => setPasswordConfirm(e.target.value)}
                value={passwordConfirm}
                type="password"
                className="form-control"
                placeholder="Confirm password"
              />
            </div>
          </div>
        </div>
        <button
          onClick={handleRegister}
          type="submit"
          className="btn btn-primary btn-block"
        >
          Sign Up
        </button>
        <span className="signup__link">
          Already have an account? <Link to="/login">Login</Link>
        </span>
      </form>
      <ToastContainer />
    </div>
  )
}

export default SignUp

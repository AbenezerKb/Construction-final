import React from "react"
import { useEffect, useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { Form, Button, Container } from "react-bootstrap"
import { FaEdit } from "react-icons/fa"

import Header from "../../layouts/Header"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { fetchSites } from "../../../api/site"
import { fetchRoles } from "../../../api/role"
import { fetchEmployee, updateEmployee } from "../../../api/employee"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function SingleEmployee() {
  const { id } = useParams()

  const [fName, setFName] = useState("")
  const [mName, setMName] = useState("")
  const [lName, setLName] = useState("")
  const [position, setPosition] = useState("")
  const [site, setSite] = useState(0)
  const [status, setStatus] = useState(0)
  const [role, setRole] = useState(0)
  const [employee, setEmployee] = useState({})

  const [allUserRole, setAllUserRole] = useState([])
  const [allSite, setAllSite] = useState([])

  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(true)

  var query = useQuery(["employee", id], () => fetchEmployee(id))

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
    if (query.data === undefined) return
    setEmployee(query.data)
    setFName(query.data.fName)
    setMName(query.data.mName)
    setLName(query.data.lName)
    setPosition(query.data.position)
    setRole(query.data.userRoleId)
    setSite(query.data.employeeSiteId)
    setStatus(query.data.status)
    setIsLoading(false)
  }, [query.data])

  const sitesQuery = useQuery("sites", fetchSites, {
    onSuccess: (data) => setAllSite(data),
  })
  const roleQuery = useQuery("roles", fetchRoles, {
    onSuccess: (data) => setAllUserRole(data),
  })

  useEffect(() => {
    setIsLoading(sitesQuery.isLoading || roleQuery.isLoading)
  }, [sitesQuery.isLoading, roleQuery.isLoading])

  useEffect(() => {
    setIsError(sitesQuery.isError || roleQuery.isError)
  }, [sitesQuery.isError, roleQuery.isError])

  const titles = useMemo(() => ["Registered Employee", "Approved Employee"], [])

  const {
    isError: isSubmitError,
    error: submitError,
    isLoading: isSubmitLoading,
    mutate: submitUpdateEmployee,
  } = useMutation(updateEmployee, {
    onSuccess: () => {
      toast.success("Employee Data Is Successfully Updated", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      employeeId: employee.employeeId,
      status: Number(status),
      fName: String(fName),
      mName: String(mName),
      lName: String(lName),
      position: String(position),
      employeeSiteId: Number(site),
      userRoleId: Number(role),
    }

    submitUpdateEmployee(data)
  }

  if (isLoading) return <LoadingSpinner />

  if (isError || isSubmitError)
    return (
      <ConnectionError
        status={
          roleQuery?.error?.response?.status ??
          sitesQuery?.error?.response?.status ??
          submitError?.response?.status
        }
      />
    )
  return (
    <>
      <Header title={titles[employee.status]} />

      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 mx-auto shadow py-5 px-5 rounded">
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>First Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={fName}
                    onChange={(e) => setFName(e.target.value)}
                  />
                </Form.Group>
              </div>
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Middle Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={mName}
                    onChange={(e) => setMName(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Last Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={lName}
                    onChange={(e) => setLName(e.target.value)}
                  />
                </Form.Group>
              </div>
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Working Site</Form.Label>
                  <Form.Select
                    value={site}
                    onChange={(e) => setSite(e.target.value)}
                  >
                    {allSite.map((site) => (
                      <option key={site.siteId} value={site.siteId}>
                        {site.name}
                      </option>
                    ))}
                  </Form.Select>
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Position</Form.Label>
                  <Form.Control
                    type="text"
                    value={position}
                    onChange={(e) => setPosition(e.target.value)}
                  />
                </Form.Group>
              </div>
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>User Role</Form.Label>
                  <Form.Select
                    value={role}
                    onChange={(e) => setRole(e.target.value)}
                  >
                    {allUserRole.map((role) => (
                      <option key={role.roleId} value={role.roleId}>
                        {role.role}
                      </option>
                    ))}
                  </Form.Select>
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col-6">
                <Form.Group className="mb-3">
                  <Form.Check
                    inline
                    label="Approved"
                    checked={status}
                    onChange={(e) => setStatus(e.target.checked)}
                    type="checkbox"
                  ></Form.Check>
                </Form.Group>
              </div>
            </div>
            <div className="row">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaEdit className="me-2 mb-1" /> Update Employee Data
                </Button>
              </div>
            </div>
          </div>
        </Form>
        <ToastContainer />
      </Container>
    </>
  )
}

export default SingleEmployee

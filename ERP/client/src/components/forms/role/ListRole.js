import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import { useQuery } from "react-query"
import { fetchRoles } from "../../../api/role"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import { useAuth } from "../../../contexts/AuthContext"

function ListRoles() {
  const [roles, setRoles] = useState([])
  const auth = useAuth()

  var { data, isLoading, isError } = useQuery("roles", fetchRoles)

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //add links to each row
      data[key].open = (
        <>
          <Link className="btn btn-teal me-2" to={`/role/${data[key].roleId}`}>
            <FaFolderOpen className="me-1 mb-1" />
            Edit
          </Link>
        </>
      )

      return null
    })
    setRoles(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Role Id.",
        accessor: "roleId", // accessor is the "key" in the data
      },
      {
        Header: "Name",
        accessor: "role",
      },
      {
        Header: "",
        accessor: "open",
        disableSortBy: true,
      },
    ],
    []
  )

  if (isLoading) return <LoadingSpinner />

  if (isError) return <ConnectionError />

  return (
    <>
      <Header title="Roles" />

      <Container className="my-3">
        <Table columns={columns} data={roles} />
      </Container>
    </>
  )
}

export default ListRoles

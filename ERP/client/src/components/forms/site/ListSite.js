import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import { useQuery } from "react-query"
import { fetchSites } from "../../../api/site"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import { useAuth } from "../../../contexts/AuthContext"

function ListSites() {
  const [sites, setSites] = useState([])
  const auth = useAuth()

  var { data, isLoading, isError } = useQuery("sites", fetchSites)

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //add links to each row
      data[key].open = (
        <>
          <Link className="btn btn-teal me-2" to={`/site/${data[key].siteId}`}>
            <FaFolderOpen className="me-1 mb-1" />
            Edit
          </Link>
        </>
      )

      return null
    })
    setSites(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Site Id.",
        accessor: "siteId", // accessor is the "key" in the data
      },
      {
        Header: "Name",
        accessor: "name",
      },
      {
        Header: "Location",
        accessor: "location",
      },
      {
        Header: "Petty Cash Limit",
        accessor: "pettyCashLimit",
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
      <Header title="Sites" />

      <Container className="my-3">
        <Table columns={columns} data={sites} />
      </Container>
    </>
  )
}

export default ListSites

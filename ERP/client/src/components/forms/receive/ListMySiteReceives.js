import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import ReceiveStatusBadge from "./fragments/ReceiveStatusBadge"
import { useQuery } from "react-query"
import { fetchMySiteReceives } from "../../../api/receive"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ValidDate from "../../../contexts/DateContext"

function ListReceives() {
  const [receives, setReceives] = useState([])

  var { data, isLoading, isError, error } = useQuery(
    "receives",
    fetchMySiteReceives
  )

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //format date
      data[key].purchaseDate = ValidDate(data[key].purchaseDate)

      //add status badge
      data[key].statusBadge = ReceiveStatusBadge(data[key].status)

      //add links to each row
      data[key].open = (
        <>
          <Link
            className="btn btn-teal me-2"
            to={`/receive/${data[key].receiveId}`}
          >
            <FaFolderOpen className="me-1 mb-1" />
            Open
          </Link>
        </>
      )

      return null
    })
    setReceives(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Receive No.",
        accessor: "receiveId", // accessor is the "key" in the data
      },
      {
        Header: "Purchase Ref",
        accessor: "purchaseId",
      },
      {
        Header: "Purchased On",
        accessor: "purchaseDate",
      },
      {
        Header: "Status",
        accessor: "statusBadge",
        sortType: (
          rowA,
          rowB //get original data of row and compare status
        ) => (rowA.original.status > rowB.original.status ? 1 : -1),
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

  if (isError) return <ConnectionError status={error?.response?.status} />

  return (
    <>
      <Header title="Receives" />

      <Container className="my-3">
        <Table columns={columns} data={receives} />
      </Container>
    </>
  )
}

export default ListReceives

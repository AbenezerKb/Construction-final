import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import BuyStatusBadge from "./fragments/BuyStatusBadge"
import { useQuery } from "react-query"
import { fetchBuys } from "../../../api/buy"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"

function ListBuys() {
  const [buys, setBuys] = useState([])

  var { data, isLoading, isError, error } = useQuery("buys", fetchBuys)

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //format date
      data[key].requestDate = new Date(data[key].requestDate).toLocaleString()

      //add status badge
      data[key].statusBadge = BuyStatusBadge(data[key].status)

      //add links to each row
      data[key].open = (
        <>
          <Link className="btn btn-teal me-2" to={`/buy/${data[key].buyId}`}>
            <FaFolderOpen className="me-1 mb-1" />
            Open
          </Link>
        </>
      )

      return null
    })
    setBuys(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Buy No.",
        accessor: "buyId", // accessor is the "key" in the data
      },
      {
        Header: "Date",
        accessor: "requestDate",
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

  if (isError) return <ConnectionError status={error?.response?.status} />

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header title="Buys" />

      <Container className="my-3">
        <Table columns={columns} data={buys} />
      </Container>
    </>
  )
}

export default ListBuys

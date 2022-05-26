import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import BulkPurchaseStatusBadge from "./fragments/BulkPurchaseStatusBadge"
import { useQuery } from "react-query"
import { fetchBulkPurchases } from "../../../api/bulkPurchase"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import { useAuth } from "../../../contexts/AuthContext"

function ListBulkPurchases() {
  const [bulkPurchases, setBulkPurchases] = useState([])
  const auth = useAuth()

  var { data, isLoading, isError, error } = useQuery(
    "bulkPurchases",
    fetchBulkPurchases
  )

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //format date
      data[key].requestDate = new Date(data[key].requestDate).toLocaleString()

      // data[key].totalCost = data[key].totalCost.toLocaleString("en-US", {
      //   style: "currency",
      //   currency: "USD",
      // })

      //add status badge
      data[key].statusBadge = BulkPurchaseStatusBadge(data[key].status)

      //add links to each row
      data[key].open = (
        <>
          <Link
            className="btn btn-teal me-2"
            to={`/bulkPurchase/${data[key].bulkPurchaseId}`}
          >
            <FaFolderOpen className="me-1 mb-1" />
            Open
          </Link>
        </>
      )

      return null
    })
    setBulkPurchases(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "BulkPurchase No.",
        accessor: "bulkPurchaseId", // accessor is the "key" in the data
      },
      {
        Header: "Requested On",
        accessor: "requestDate",
      },
      {
        Header: "Total Cost",
        accessor: "totalPurchaseCost",
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
      <Header title="BulkPurchases" />

      <Container className="my-3">
        <Table columns={columns} data={bulkPurchases} />
      </Container>
    </>
  )
}

export default ListBulkPurchases

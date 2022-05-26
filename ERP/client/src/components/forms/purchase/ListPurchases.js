import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import PurchaseStatusBadge from "./fragments/PurchaseStatusBadge"
import { useQuery } from "react-query"
import { fetchPurchases } from "../../../api/purchase"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import { useAuth } from "../../../contexts/AuthContext"

function ListPurchases() {
  const [purchases, setPurchases] = useState([])
  const auth = useAuth()
  const requestData = {
    receivingSiteId: auth.data.employee.employeeSiteId,
  }
  var { data, isLoading, isError, error } = useQuery("purchases", () =>
    fetchPurchases(requestData)
  )

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //format date
      data[key].requestDate = new Date(data[key].requestDate).toLocaleString()

      //add status badge
      data[key].statusBadge = PurchaseStatusBadge(data[key].status)

      //add links to each row
      data[key].open = (
        <>
          <Link
            className="btn btn-teal me-2"
            to={`/purchase/${data[key].purchaseId}`}
          >
            <FaFolderOpen className="me-1 mb-1" />
            Open
          </Link>
        </>
      )

      return null
    })
    setPurchases(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Purchase No.",
        accessor: "purchaseId", // accessor is the "key" in the data
      },
      {
        Header: "Requested On",
        accessor: "requestDate",
      },
      {
        Header: "Receiving Site",
        accessor: "receivingSite.name",
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
      <Header title="Purchases" />

      <Container className="my-3">
        <Table columns={columns} data={purchases} />
      </Container>
    </>
  )
}

export default ListPurchases

import React, { useEffect, useState } from "react"
import { Link } from "react-router-dom"
import { FaFolderOpen } from "react-icons/fa"
import Table from "../../fragments/Table"
import Header from "../../layouts/Header"
import { Container } from "react-bootstrap"
import { useQuery } from "react-query"
import { fetchEquipments } from "../../../api/item"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import { useAuth } from "../../../contexts/AuthContext"

function ListEquipments() {
  const [equipments, setEquipments] = useState([])
  const auth = useAuth()

  var { data, isLoading, isError } = useQuery("equipments", fetchEquipments)

  useEffect(() => {
    if (data === undefined) return

    Object.keys(data).map((key, index) => {
      //add links to each row
      data[key].open = (
        <>
          <Link
            className="btn btn-teal me-2"
            to={`/item/equipment/${data[key].itemId}`}
          >
            <FaFolderOpen className="me-1 mb-1" />
            Edit
          </Link>
        </>
      )

      return null
    })
    setEquipments(data)
  }, [data])

  const columns = React.useMemo(
    () => [
      {
        Header: "Equipment Id.",
        accessor: "itemId", // accessor is the "key" in the data
      },
      {
        Header: "Name",
        accessor: "name",
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
      <Header title="Equipments" />

      <Container className="my-3">
        <Table columns={columns} data={equipments} />
      </Container>
    </>
  )
}

export default ListEquipments

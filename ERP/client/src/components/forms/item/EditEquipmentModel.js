import React from "react"
import Header from "../../layouts/Header"
import { useEffect, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { useParams } from "react-router-dom"
import { FaEdit } from "react-icons/fa"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { editEquipmentModel, fetchEquipmentModel } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EditEquipmentModel() {
  const { equipmentModelId } = useParams()
  const modelId = equipmentModelId

  const [name, setName] = useState("")
  const [cost, setCost] = useState(0)
  const [equipmentItem, setEquipmentItem] = useState(0)

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  var query = useQuery(
    ["model", modelId],
    () => modelId && fetchEquipmentModel(modelId)
  )

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
    setName(query.data.name)
    setCost(query.data.cost)
    setEquipmentItem(query.data.equipment.item.name)
    setIsLoading(false)
  }, [query.data])

  const {
    isError: isSubmitError,
    isLoading: isSubmitLoading,
    mutate: submitEquipmentModelRequest,
  } = useMutation(editEquipmentModel, {
    onSuccess: () => {
      toast.success("Equipment Model Is Successfully Updated", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      equipmentModelId: equipmentModelId,
      name: String(name),
      cost: Number(cost),
    }

    submitEquipmentModelRequest(data)
  }

  if (isError || isSubmitError) return <ConnectionError />

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header title="New Equipment Model" />

      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 shadow py-5 px-5 mx-auto rounded">
            <div className="row">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Item</Form.Label>
                  <Form.Control
                    type="text"
                    value={equipmentItem}
                    readOnly
                  ></Form.Control>
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Equipment Model Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Model Cost</Form.Label>
                  <Form.Control
                    min="1"
                    type="number"
                    value={cost}
                    onChange={(e) => setCost(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaEdit className="me-2 mb-1" /> Update Equipment Model
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

export default EditEquipmentModel

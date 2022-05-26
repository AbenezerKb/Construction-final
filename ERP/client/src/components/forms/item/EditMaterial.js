import React from "react"
import Header from "../../layouts/Header"
import { useEffect, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { useParams } from "react-router-dom"
import { FaEdit } from "react-icons/fa"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { editMaterial, fetchItem } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EditMaterial() {
  const { itemId } = useParams()

  const [name, setName] = useState("")
  const [spec, setSpec] = useState("")
  const [unit, setUnit] = useState("")
  const [isTransferable, setIsTransferable] = useState(false)
  const [cost, setCost] = useState(0)

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  var query = useQuery(["item", itemId], () => itemId && fetchItem(itemId))

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
    setUnit(query.data.material.unit)
    setSpec(query.data.material.spec)
    setIsTransferable(query.data.material.isTransferable)
    setCost(query.data.material.cost)
    setIsLoading(false)
  }, [query.data])

  const {
    isError: isSubmitError,
    isLoading: isSubmitLoading,
    mutate: submitMaterialRequest,
  } = useMutation(editMaterial, {
    onSuccess: (res) => {
      toast.success("Item Is Successfully Updated", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      itemId: itemId,
      name: String(name),
      spec: String(spec),
      unit: String(unit),
      cost: Number(cost),
      isTransferable: isTransferable,
    }

    submitMaterialRequest(data)
  }

  if (isLoading) return <LoadingSpinner />

  if (isError || isSubmitError) return <ConnectionError />

  return (
    <>
      <Header title="New Material" />

      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 mx-auto shadow py-5 px-5 rounded">
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Item Name</Form.Label>
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
                  <Form.Label>Item Specification</Form.Label>
                  <Form.Control
                    as="textarea"
                    value={spec}
                    onChange={(e) => setSpec(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Item Unit</Form.Label>
                  <Form.Control
                    type="text"
                    value={unit}
                    onChange={(e) => setUnit(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Item Cost</Form.Label>
                  <Form.Control
                    min="0"
                    type="number"
                    value={cost}
                    onChange={(e) => setCost(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>

            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Check
                    checked={isTransferable}
                    onChange={() => setIsTransferable((prev) => !prev)}
                    id="transferable"
                    label="Material Can Be Transferred Between Sites"
                  />
                </Form.Group>
              </div>
            </div>

            <div className="row">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaEdit className="me-2 mb-1" /> Update Item
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

export default EditMaterial

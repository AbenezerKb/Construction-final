import { useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import Header from "../../layouts/Header"
import { ToastContainer, toast } from "react-toastify"
import { useMutation } from "react-query"
import { addMaterial } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function MaterialItem() {
  const [name, setName] = useState("")
  const [spec, setSpec] = useState("")
  const [unit, setUnit] = useState("")
  const [isTransferable, setIsTransferable] = useState(false)
  const [cost, setCost] = useState(0)

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  const toastOption = {
    position: "bottom-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    theme: "dark",
  }

  const {
    isError: isSubmitError,
    isLoading: isSubmitLoading,
    mutate: submitMaterialRequest,
  } = useMutation(addMaterial, {
    onSuccess: (res) => {
      toast.success("Item Is Successfully Added", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      name: String(name),
      spec: String(spec),
      unit: String(unit),
      cost: Number(cost),
      isTransferable: isTransferable,
    }

    setName("")
    setSpec("")
    setUnit("")
    setCost(0)
    setIsTransferable(false)

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
                  <FaPlus className="me-2 mb-1" /> Add Item
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

export default MaterialItem

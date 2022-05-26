import { useEffect, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import Header from "../../layouts/Header"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { addEquipmentModel } from "../../../api/item"
import { fetchEquipments } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EquipmentModel() {
  const [name, setName] = useState("")
  const [cost, setCost] = useState(0)
  const [defaultItem, setDefaultItem] = useState(0)
  const [equipmentItem, setEquipmentItem] = useState(0)
  const [allEquipmentItems, setAllEquipmentItems] = useState([])

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  const equipmentItemsQuery = useQuery("equipmentItems", fetchEquipments, {
    onSuccess: (data) => {
      setAllEquipmentItems(data)
      setEquipmentItem(data[0].itemId)
      setDefaultItem(data[0].itemId)
    },
  })

  useEffect(() => {
    setIsLoading(equipmentItemsQuery.isLoading)
  }, [equipmentItemsQuery.isLoading])

  useEffect(() => {
    setIsError(equipmentItemsQuery.isError)
  }, [equipmentItemsQuery.isError])

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
    mutate: submitEquipmentModelRequest,
  } = useMutation(addEquipmentModel, {
    onSuccess: () => {
      toast.success("Equipment Model Is Successfully Added", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      name: String(name),
      itemId: Number(equipmentItem),
      cost: Number(cost),
    }

    setName("")
    setEquipmentItem(defaultItem)
    setCost(0)

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
                  <Form.Select
                    value={equipmentItem}
                    onChange={(e) => setEquipmentItem(e.target.value)}
                  >
                    {allEquipmentItems.map((item) => (
                      <option key={item.itemId} value={item.itemId}>
                        {item.name}
                      </option>
                    ))}
                  </Form.Select>
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
                  <FaPlus className="me-2 mb-1" /> Add Equipment Model
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

export default EquipmentModel

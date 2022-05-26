import { useEffect, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import Header from "../../layouts/Header"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { addEquipment } from "../../../api/item"
import { fetchEquipmentCategories } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EquipmentItem() {
  const [name, setName] = useState("")
  const [unit, setUnit] = useState("")
  const [description, setDescription] = useState("")
  const [defaultEquipmentCategory, setDefaultEquipmentCategory] = useState(0)
  const [equipmentCategory, setEquipmentCategory] = useState(0)
  const [allCategories, setAllCategories] = useState([])

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  const categoriesQuery = useQuery("categories", fetchEquipmentCategories, {
    onSuccess: (data) => {
      setAllCategories(data)
      setEquipmentCategory(data[0].equipmentCategoryId)
      setDefaultEquipmentCategory(data[0].equipmentCategoryId)
    },
  })

  useEffect(() => {
    setIsLoading(categoriesQuery.isLoading)
  }, [categoriesQuery.isLoading])

  useEffect(() => {
    setIsError(categoriesQuery.isError)
  }, [categoriesQuery.isError])

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
    mutate: submitEquipmentRequest,
  } = useMutation(addEquipment, {
    onSuccess: () => {
      toast.success("Item Is Successfully Added", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      name: String(name),
      unit: String(unit),
      description: String(description),
      equipmentCategoryId: Number(equipmentCategory),
    }

    setName("")
    setUnit("")
    setDescription("")
    setEquipmentCategory(defaultEquipmentCategory)

    submitEquipmentRequest(data)
  }

  if (isLoading) return <LoadingSpinner />

  if (isError || isSubmitError) return <ConnectionError />

  return (
    <>
      <Header title="New Equipment" />

      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 shadow py-5 px-5 mx-auto rounded">
            <div className="row">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Equipment Category</Form.Label>
                  <Form.Select
                    value={equipmentCategory}
                    onChange={(e) => setEquipmentCategory(e.target.value)}
                  >
                    {allCategories.map((category) => (
                      <option
                        key={category.equipmentCategoryId}
                        value={category.equipmentCategoryId}
                      >
                        {category.name}
                      </option>
                    ))}
                  </Form.Select>
                </Form.Group>
              </div>
            </div>
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
                  <Form.Label>Item Description</Form.Label>
                  <Form.Control
                    as="textarea"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
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

export default EquipmentItem

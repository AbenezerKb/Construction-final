import React from "react"
import Header from "../../layouts/Header"
import { useEffect, useState } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { useParams } from "react-router-dom"
import { FaEdit } from "react-icons/fa"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import {
  editEquipment,
  fetchItem,
  fetchEquipmentCategories,
} from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EditEquipment() {
  const { itemId } = useParams()

  const [name, setName] = useState("")
  const [unit, setUnit] = useState("")
  const [description, setDescription] = useState("")
  const [equipmentCategory, setEquipmentCategory] = useState(0)
  const [equipmentModels, setEquipmentModels] = useState([])

  const [allCategories, setAllCategories] = useState([])

  const [isLoading, setIsLoading] = useState(false)
  const [isError, setIsError] = useState(false)

  var query = useQuery(["item", itemId], () => itemId && fetchItem(itemId))

  const categoriesQuery = useQuery("categories", fetchEquipmentCategories, {
    onSuccess: (data) => {
      setAllCategories(data)
    },
  })

  useEffect(() => {
    setIsLoading(categoriesQuery.isLoading)
  }, [categoriesQuery.isLoading])

  useEffect(() => {
    setIsError(categoriesQuery.isError)
  }, [categoriesQuery.isError])

  useEffect(() => {
    if (query.data === undefined) return
    setName(query.data.name)
    setUnit(query.data.equipment.unit)
    setDescription(query.data.equipment.description)
    setEquipmentCategory(query.data.equipment.equipmentCategoryId)
    setEquipmentModels(query.data.equipment.equipmentModels)
    setIsLoading(false)
  }, [query.data])

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
  } = useMutation(editEquipment, {
    onSuccess: () => {
      toast.success("Item Is Successfully Updated", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      itemId: itemId,
      name: String(name),
      unit: String(unit),
      description: String(description),
      equipmentCategoryId: Number(equipmentCategory),
    }

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

export default EditEquipment

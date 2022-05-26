import { useState, useRef } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import Header from "../../layouts/Header"
import { ToastContainer, toast } from "react-toastify"
import { useMutation } from "react-query"
import { addEquipmentCategory } from "../../../api/item"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import { uploadFileAPI } from "../../../api/file"

function EquipmentCategory() {
  const [name, setName] = useState("")
  const fileRef = useRef()
  const [isUploadError, setIsUploadError] = useState(false)

  const toastOption = {
    position: "bottom-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    theme: "dark",
    // progress: undefined
  }

  async function uploadFile(file) {
    const data = new FormData()
    data.append("file", file)

    try {
      return await uploadFileAPI(data)
    } catch {
      setIsUploadError(true)
    }
  }

  const {
    isError: isSubmitError,
    error: submitError,
    isLoading: isSubmitLoading,
    mutate: submitEquipmentCategoryRequest,
  } = useMutation(addEquipmentCategory, {
    onSuccess: (res) => {
      toast.success("Equipment Category Is Successfully Added", toastOption)
    },
  })

  async function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return
    const fileName = await uploadFile(fileRef.current.files[0])

    if (!fileName) return

    var data = {
      name: String(name),
      fileName,
    }

    fileRef.current.value = ""
    setName("")

    submitEquipmentCategoryRequest(data)
  }

  if (isUploadError || isSubmitError)
    return <ConnectionError status={submitError?.response?.status} />

  return (
    <>
      <Header title="New Equipment Category" />

      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 shadow py-5 px-5 mx-auto rounded">
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Equipment Category Name</Form.Label>
                  <Form.Control
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Equipment Category Image</Form.Label>
                  <Form.Control
                    type="file"
                    ref={fileRef}
                    required
                    accept=".png, .jpeg, .jpg"
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaPlus className="me-2 mb-1" /> Add Equipment Category
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

export default EquipmentCategory

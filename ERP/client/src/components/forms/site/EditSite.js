import { useState, useEffect } from "react"
import { Form, Button, Container } from "react-bootstrap"
import { FaEdit } from "react-icons/fa"
import Header from "../../layouts/Header"
import { fetchSite, editSite } from "../../../api/site"
import { useParams } from "react-router-dom"
import { useQuery, useMutation } from "react-query"
import { ToastContainer, toast } from "react-toastify"
import ConnectionError from "../../fragments/ConnectionError"
import LoadingSpinner from "../../fragments/LoadingSpinner"

function EditSite() {
  const { id } = useParams()

  const [name, setName] = useState("")
  const [location, setLocation] = useState("")
  const [pettyCashLimit, setPettyCashLimit] = useState(1000)

  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(true)

  var query = useQuery(["role", id], () => fetchSite(id))

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
    setLocation(query.data.location)
    setPettyCashLimit(query.data.pettyCashLimit)
    setIsLoading(false)
  }, [query.data])

  const {
    isError: isSubmitError,
    isLoading: isSubmitLoading,
    error: submitError,
    mutate: submitEditSite,
  } = useMutation(editSite, {
    onSuccess: (res) => {
      toast.success("Site Is Successfully Updated", toastOption)
    },
  })

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    var data = {
      siteId: id,
      name: String(name),
      location: String(location),
      pettyCashLimit: Number(pettyCashLimit),
    }

    submitEditSite(data)
  }

  if (isLoading) return <LoadingSpinner />

  if (isSubmitError)
    return <ConnectionError status={submitError?.response?.status} />

  return (
    <>
      <Header title="Edit Site" />
      <Container className="my-3 align-self-center">
        <Form onSubmit={submit}>
          <div className="col-10 mx-auto shadow py-5 px-5 rounded">
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Site Name</Form.Label>
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
                  <Form.Label>Location</Form.Label>
                  <Form.Control
                    type="text"
                    value={location}
                    onChange={(e) => setLocation(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>
            <div className="row ">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Petty Cash Limit</Form.Label>
                  <Form.Control
                    type="number"
                    value={pettyCashLimit}
                    onChange={(e) => setPettyCashLimit(e.target.value)}
                  />
                </Form.Group>
              </div>
            </div>

            <div className="row">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaEdit className="me-2 mb-1" /> Update Site
                </Button>
              </div>
            </div>
          </div>
        </Form>
        <ToastContainer />
      </Container>
      <ToastContainer />
    </>
  )
}

export default EditSite

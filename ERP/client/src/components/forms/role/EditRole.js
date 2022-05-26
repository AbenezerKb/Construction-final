import React from "react"
import Header from "../../layouts/Header"
import Role from "../../../models/Role"
import { useState, useEffect } from "react"
import { Form, Button, Container, InputGroup } from "react-bootstrap"
import { useParams } from "react-router-dom"
import { FaEdit } from "react-icons/fa"
import { ToastContainer, toast } from "react-toastify"
import { useQuery, useMutation } from "react-query"
import { fetchRole, editRole } from "../../../api/role"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"

function EditRole() {
  const { id } = useParams()

  const [role, setRole] = useState(new Role())
  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(true)

  var query = useQuery(["role", id], () => fetchRole(id))

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
    setRole(query.data)
    setIsLoading(false)
  }, [query.data])

  const {
    isError: isSubmitError,
    isLoading: isSubmitLoading,
    error: submitError,
    mutate: submitEditRole,
  } = useMutation(editRole, {
    onSuccess: (res) => {
      toast.success("Role Is Successfully Updated", toastOption)
    },
  })

  const handleChange = (event) => {
    setRole((state) => {
      let newRole = { ...state }
      newRole[event.target.name] = event.target.checked
      return newRole
    })
  }

  const valueChange = (event) => {
    setRole((state) => {
      let newRole = { ...state }
      newRole[event.target.name] = event.target.value
      return newRole
    })
  }

  function submit(e) {
    e.preventDefault()
    if (isSubmitLoading) return

    submitEditRole(role)
  }

  if (isLoading) return <LoadingSpinner />

  if (isSubmitError)
    return <ConnectionError status={submitError?.response?.status} />

  return (
    <>
      <Header title="Edit Role" />

      <Container className="my-3">
        <Form onSubmit={submit}>
          <div>
            <div className="row">
              <div className="mx-4 col">
                <Form.Group>
                  <InputGroup className="mb-3">
                    <Form.Label className="col-3 pt-2 h5">Role Name</Form.Label>
                    <Form.Control
                      type="text"
                      name="role"
                      value={role.role}
                      onChange={valueChange}
                      required
                    />
                  </InputGroup>
                </Form.Group>
              </div>
            </div>

            <div className="row py-2 text-center">
              <h4>Set User Permissions</h4>
            </div>
            <div className="my-2">
              <h5 className="px-4">User Management</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Edit User Acconunt"
                      type="checkbox"
                      name="canEditUser"
                      checked={role.canEditUser}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Reports (Is User Finance)"
                      type="checkbox"
                      name="isFinance"
                      checked={role.isFinance}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Purchase</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Purchase Requests"
                      type="checkbox"
                      name="canViewPurchase"
                      checked={role.canViewPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Purchase"
                      type="checkbox"
                      name="canRequestPurchase"
                      checked={role.canRequestPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Check Purchase"
                      type="checkbox"
                      name="canCheckPurchase"
                      checked={role.canCheckPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Purchase"
                      type="checkbox"
                      name="canApprovePurchase"
                      checked={role.canApprovePurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Confirm Purchase"
                      type="checkbox"
                      name="canConfirmPurchase"
                      checked={role.canConfirmPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Bulk Purchase</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Bulk Purchase Requests"
                      type="checkbox"
                      name="canViewBulkPurchase"
                      checked={role.canViewBulkPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Bulk Purchase"
                      type="checkbox"
                      name="canRequestBulkPurchase"
                      checked={role.canRequestBulkPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Bulk Purchase"
                      type="checkbox"
                      name="canApproveBulkPurchase"
                      checked={role.canApproveBulkPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Confirm Bulk Purchase"
                      type="checkbox"
                      name="canConfirmBulkPurchase"
                      checked={role.canConfirmBulkPurchase}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Buy</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Buy Requests"
                      type="checkbox"
                      name="canViewBuy"
                      checked={role.canViewBuy}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Buy"
                      type="checkbox"
                      name="canRequestBuy"
                      checked={role.canRequestBuy}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Check Buy"
                      type="checkbox"
                      name="canCheckBuy"
                      checked={role.canCheckBuy}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Buy"
                      type="checkbox"
                      name="canApproveBuy"
                      checked={role.canApproveBuy}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Confirm Buy"
                      type="checkbox"
                      name="canConfirmBuy"
                      checked={role.canConfirmBuy}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Receive</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Receive Requests"
                      type="checkbox"
                      name="canViewReceive"
                      checked={role.canViewReceive}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Receive Item"
                      type="checkbox"
                      name="canReceive"
                      checked={role.canReceive}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Receive"
                      type="checkbox"
                      name="canApproveReceive"
                      checked={role.canApproveReceive}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Issue</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Issue Requests"
                      type="checkbox"
                      name="canViewIssue"
                      checked={role.canViewIssue}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request An Issue"
                      type="checkbox"
                      name="canRequestIssue"
                      checked={role.canRequestIssue}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve An Issue"
                      type="checkbox"
                      name="canApproveIssue"
                      checked={role.canApproveIssue}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Hand An Issue"
                      type="checkbox"
                      name="canHandIssue"
                      checked={role.canHandIssue}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Transfer</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Transfer Requests"
                      type="checkbox"
                      name="canViewTransfer"
                      checked={role.canViewTransfer}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Transfer"
                      type="checkbox"
                      name="canRequestTransfer"
                      checked={role.canRequestTransfer}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Receive Transfer"
                      type="checkbox"
                      name="canReceiveTransfer"
                      checked={role.canReceiveTransfer}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Transfer"
                      type="checkbox"
                      name="canApproveTransfer"
                      checked={role.canApproveTransfer}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Send Transfer"
                      type="checkbox"
                      name="canSendTransfer"
                      checked={role.canSendTransfer}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Borrow</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Borrow Requests"
                      type="checkbox"
                      name="canViewBorrow"
                      checked={role.canViewBorrow}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Borrow"
                      type="checkbox"
                      name="canRequestBorrow"
                      checked={role.canRequestBorrow}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Borrow"
                      type="checkbox"
                      name="canApproveBorrow"
                      checked={role.canApproveBorrow}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Hand Borrow"
                      type="checkbox"
                      name="canHandBorrow"
                      checked={role.canHandBorrow}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Return Borrow"
                      type="checkbox"
                      name="canReturnBorrow"
                      checked={role.canReturnBorrow}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Maintenance</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can View Maintenance Requests"
                      type="checkbox"
                      name="canViewMaintenance"
                      checked={role.canViewMaintenance}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Maintenance"
                      type="checkbox"
                      name="canRequestMaintenance"
                      checked={role.canRequestMaintenance}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Fix Maintenance"
                      type="checkbox"
                      name="canFixMaintenance"
                      checked={role.canFixMaintenance}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Maintenance"
                      type="checkbox"
                      name="canApproveMaintenance"
                      checked={role.canApproveMaintenance}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="my-4">
              <h5 className="px-4">Notification</h5>
              <div className="row mx-4">
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Get Stock Notification"
                      type="checkbox"
                      name="canGetStockNotification"
                      checked={role.canGetStockNotification}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="row mx-4">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaEdit className="me-2 mb-1" /> Update Role
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

export default EditRole

import Header from "../../layouts/Header"
import Role from "../../../models/Role"
import { useState, useMemo, useEffect } from "react"
import { Form, Button, Container, InputGroup } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import { ToastContainer, toast } from "react-toastify"
import { useMutation } from "react-query"
import { addRole } from "../../../api/role"
import ConnectionError from "../../fragments/ConnectionError"

function NewRole() {
  const [role, setRole] = useState(new Role())
  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

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
    mutate: submitNewRole,
  } = useMutation(addRole, {
    onSuccess: (res) => {
      toast.success("Role Is Successfully Added", toastOption)
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

    submitNewRole(role)

    setRole(new Role())
  }

  if (isLoading) return <LoadingSpinner />

  if (isSubmitError)
    return <ConnectionError status={submitError?.response?.status} />

  return (
    <>
      <Header title="Add New Role" />

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
                      name="Role"
                      value={role.Role}
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
                      name="CanEditUser"
                      checked={role.CanEditUser}
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
                      name="IsFinance"
                      checked={role.IsFinance}
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
                      name="CanViewPurchase"
                      checked={role.CanViewPurchase}
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
                      name="CanRequestPurchase"
                      checked={role.CanRequestPurchase}
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
                      name="CanCheckPurchase"
                      checked={role.CanCheckPurchase}
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
                      name="CanApprovePurchase"
                      checked={role.CanApprovePurchase}
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
                      name="CanConfirmPurchase"
                      checked={role.CanConfirmPurchase}
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
                      name="CanViewBulkPurchase"
                      checked={role.CanViewBulkPurchase}
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
                      name="CanRequestBulkPurchase"
                      checked={role.CanRequestBulkPurchase}
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
                      name="CanApproveBulkPurchase"
                      checked={role.CanApproveBulkPurchase}
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
                      name="CanConfirmBulkPurchase"
                      checked={role.CanConfirmBulkPurchase}
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
                      name="CanViewBuy"
                      checked={role.CanViewBuy}
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
                      name="CanRequestBuy"
                      checked={role.CanRequestBuy}
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
                      name="CanCheckBuy"
                      checked={role.CanCheckBuy}
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
                      name="CanApproveBuy"
                      checked={role.CanApproveBuy}
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
                      name="CanConfirmBuy"
                      checked={role.CanConfirmBuy}
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
                      name="CanViewReceive"
                      checked={role.CanViewReceive}
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
                      name="CanReceive"
                      checked={role.CanReceive}
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
                      name="CanApproveReceive"
                      checked={role.CanApproveReceive}
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
                      name="CanViewIssue"
                      checked={role.CanViewIssue}
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
                      name="CanRequestIssue"
                      checked={role.CanRequestIssue}
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
                      name="CanApproveIssue"
                      checked={role.CanApproveIssue}
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
                      name="CanHandIssue"
                      checked={role.CanHandIssue}
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
                      name="CanViewTransfer"
                      checked={role.CanViewTransfer}
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
                      name="CanRequestTransfer"
                      checked={role.CanRequestTransfer}
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
                      name="CanReceiveTransfer"
                      checked={role.CanReceiveTransfer}
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
                      name="CanApproveTransfer"
                      checked={role.CanApproveTransfer}
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
                      name="CanSendTransfer"
                      checked={role.CanSendTransfer}
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
                      name="CanViewBorrow"
                      checked={role.CanViewBorrow}
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
                      name="CanRequestBorrow"
                      checked={role.CanRequestBorrow}
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
                      name="CanApproveBorrow"
                      checked={role.CanApproveBorrow}
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
                      name="CanHandBorrow"
                      checked={role.CanHandBorrow}
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
                      name="CanReturnBorrow"
                      checked={role.CanReturnBorrow}
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
                      name="CanViewMaintenance"
                      checked={role.CanViewMaintenance}
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
                      name="CanRequestMaintenance"
                      checked={role.CanRequestMaintenance}
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
                      name="CanFixMaintenance"
                      checked={role.CanFixMaintenance}
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
                      name="CanApproveMaintenance"
                      checked={role.CanApproveMaintenance}
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
                      name="CanGetStockNotification"
                      checked={role.CanGetStockNotification}
                      onChange={handleChange}
                    ></Form.Check>
                  </Form.Group>
                </div>
              </div>
            </div>
            <div className="row mx-4">
              <div className="d-grid">
                <Button type="submit" className="btn-teal">
                  <FaPlus className="me-2 mb-1" /> Add Role
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

export default NewRole

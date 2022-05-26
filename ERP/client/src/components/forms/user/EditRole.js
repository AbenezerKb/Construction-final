import Header from "../../layouts/Header"
import Role from "../../../models/Role"
import { useState, useMemo, useEffect } from "react"
import { Form, Button, Container, InputGroup } from "react-bootstrap"
import { FaPlus } from "react-icons/fa"
import ToggleBox from "../../layouts/ToggleBox"
import { ToastContainer, toast } from "react-toastify"
import { useMutation } from "react-query"
import { addRole } from "../../../api/role"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import AlertNotification from "../../fragments/AlertNotification"
import { useNavigate } from "react-router-dom"

function NewRole() {
  const tempRole = new Role()
  const [role, setRole] = useState(new Role())
  const navigate = useNavigate()

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
    mutate: submitNewRole,
  } = useMutation(addRole, {
    onSuccess: (res) => {
      toast.success("Role Is Successfully Added", toastOption)
    },
  })

  function valueChanged(e) {
    tempRole[e.target.name] = e.target.checked
  }

  function submit(e) {
    e.preventDefault()

    submitNewRole(tempRole)

    role = new Role()
  }

  //if (isError || isSubmitError) return <ConnectionError />

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
                      onChange={(e) => {
                        tempRole.Role = e.target.value
                      }}
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
                      onChange={valueChanged}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanViewPurchase = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Purchase"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestPurchase = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanCheckPurchase = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Purchase"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApprovePurchase = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanConfirmPurchase = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanViewBulkPurchase = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Bulk Purchase"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestBulkPurchase = e.target.checked)
                      }
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
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveBulkPurchase = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Confirm Bulk Purchase"
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanConfirmBulkPurchase = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanViewBuy = e.target.checked)}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Buy"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) => (role.CanRequestBuy = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanCheckBuy = e.target.checked)}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Buy"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) => (role.CanApproveBuy = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanConfirmBuy = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanViewReceive = e.target.checked)}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Receive Item"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) => (role.CanReceive = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveReceive = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanViewIssue = e.target.checked)}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request An Issue"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestIssue = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveIssue = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Hand An Issue"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) => (role.CanHandIssue = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanViewTransfer = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Transfer"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestTransfer = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanReceiveTransfer = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Transfer"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveTransfer = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanSendTransfer = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) => (role.CanViewBorrow = e.target.checked)}
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Borrow"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestBorrow = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveBorrow = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Hand Borrow"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) => (role.CanHandBorrow = e.target.checked)}
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanReturnBorrow = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanViewMaintenance = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Request Maintenance"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanRequestMaintenance = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanFixMaintenance = e.target.checked)
                      }
                    ></Form.Check>
                  </Form.Group>
                </div>
                <div className="col-6">
                  <Form.Group className="mb-3">
                    <Form.Check
                      inline
                      label="Can Approve Maintenance"
                      name="bool1"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanApproveMaintenance = e.target.checked)
                      }
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
                      name="bool"
                      type="checkbox"
                      onChange={(e) =>
                        (role.CanGetStockNotification = e.target.checked)
                      }
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

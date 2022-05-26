import { Form, InputGroup, Table } from "react-bootstrap"
import PropTypes from "prop-types"
import { useQuery } from "react-query"
import { fetchEquipmentModel } from "../../../../api/item"
import { ITEMTYPE } from "../../../../Constants"

function ReceiveViewItem({ addedItems, index }) {
  var modelQuery = useQuery(
    ["model", addedItems[index].equipmentModelId],
    () =>
      addedItems[index].equipmentModelId &&
      fetchEquipmentModel(addedItems[index].equipmentModelId)
  )

  return (
    <>
      <div className="shadow pt-3 px-4 mb-4 rounded">
        <div className=" row justify-content-between">
          <div className="col-auto">
            {addedItems[index].type == ITEMTYPE.MATERIAL ? (
              <h1 className="display-6 fs-4">Material {index + 1}</h1>
            ) : (
              <h1 className="display-6 fs-4">Equipment {index + 1}</h1>
            )}
          </div>
        </div>

        <div className="row">
          <div className="col">
            <Form.Group className="mb-3">
              <Form.Label>Item</Form.Label>
              <Form.Control
                type="text"
                readOnly
                value={addedItems[index].name}
              />
            </Form.Group>
          </div>
          {addedItems[index].type == ITEMTYPE.EQUIPMENT && (
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Model</Form.Label>
                <Form.Control
                  type="text"
                  name="cost"
                  value={modelQuery?.data?.name || ""}
                  readOnly
                />
              </Form.Group>
            </div>
          )}
        </div>

        <div className="row">
          {addedItems[index].type == ITEMTYPE.MATERIAL ? (
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Specification</Form.Label>
                <Form.Control
                  as="textarea"
                  name="spec"
                  rows={3}
                  value={addedItems[index].spec}
                  readOnly
                />
              </Form.Group>
            </div>
          ) : (
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Description</Form.Label>
                <Form.Control
                  as="textarea"
                  name="spec"
                  rows={3}
                  value={addedItems[index].description}
                  readOnly
                />
              </Form.Group>
            </div>
          )}
          <div className="col">
            {addedItems[index].qtyReceived > 0 && (
              <div className="row">
                <div className="col">
                  <Form.Group className="mb-3">
                    <Form.Label>Quantity Received</Form.Label>
                    <Form.Control
                      type="text"
                      name="qtyReceived"
                      value={addedItems[index].qtyReceived}
                      readOnly
                    />
                  </Form.Group>
                </div>
              </div>
            )}
            <div className="row">
              <div className="col">
                <Form.Group className="mb-3">
                  <Form.Label>Receive Remark</Form.Label>
                  <Form.Control
                    type="text"
                    readOnly
                    value={addedItems[index].receiveRemark}
                  />
                </Form.Group>
              </div>
            </div>
          </div>
        </div>
      </div>
      {addedItems[index].type == ITEMTYPE.MATERIAL ? (
        <tr className="d-none d-print-table-row">
          <th>{index + 1}</th>
          <td>{addedItems[index].name}</td>
          <td>{addedItems[index].spec}</td>
          <td>{addedItems[index].qtyReceived}</td>
          <td>{addedItems[index].cost}</td>
          <td>{addedItems[index].totalCost}</td>
          <td>{addedItems[index].receivetRemark}</td>
        </tr>
      ) : (
        <tr className="d-none d-print-table-row">
          <th>{index + 1}</th>
          <td>{addedItems[index].name}</td>
          <td>{modelQuery?.data?.name || ""}</td>
          <td>{addedItems[index].desc}</td>
          <td>{addedItems[index].qtyReceived}</td>
          <td>{addedItems[index].cost}</td>
          <td>{addedItems[index].totalCost}</td>
          <td>{addedItems[index].receivetRemark}</td>
        </tr>
      )}
    </>
  )
}

ReceiveViewItem.propTypes = {
  addedItems: PropTypes.array.isRequired,
  index: PropTypes.number.isRequired,
}

export default ReceiveViewItem

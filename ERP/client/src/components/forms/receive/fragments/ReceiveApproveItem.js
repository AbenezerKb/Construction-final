import { Form } from "react-bootstrap"
import PropTypes from "prop-types"
import { useQuery } from "react-query"
import { fetchEquipmentModel } from "../../../../api/item"
import { ITEMTYPE } from "../../../../Constants"

function ReceiveApproveItem({ addedItems, setAddedItems, index }) {
  var modelQuery = useQuery(
    ["model", addedItems[index].equipmentModelId],
    () =>
      addedItems[index].equipmentModelId &&
      fetchEquipmentModel(addedItems[index].equipmentModelId)
  )

  return (
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
            <Form.Control type="text" readOnly value={addedItems[index].name} />
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
          <div className="row">
            <Form.Group className="mb-3">
              <Form.Label>Quantity Received</Form.Label>
              <Form.Control
                type="text"
                name="qty"
                readOnly
                value={addedItems[index].qtyReceived}
              />
            </Form.Group>
          </div>

          <div className="row">
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
  )
}

export default ReceiveApproveItem

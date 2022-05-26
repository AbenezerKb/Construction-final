import { useEffect } from "react"
import { Form, InputGroup, Table } from "react-bootstrap"
import { useQuery } from "react-query"
import { fetchEquipmentModel } from "../../../../api/item"
import { ITEMTYPE } from "../../../../Constants"

function PurchaseConfirmEquipment({ addedItems, setAddedItems, index }) {
  var modelQuery = useQuery(
    ["model", addedItems[index].equipmentModelId],
    () =>
      addedItems[index].equipmentModelId &&
      fetchEquipmentModel(addedItems[index].equipmentModelId)
  )

  //set purchased qty to approved as default
  useEffect(() => {
    const addedItemsCpy = [...addedItems]
    addedItemsCpy[index].qtyPurchased = addedItemsCpy[index].qtyApproved
    setAddedItems(addedItemsCpy)
  }, [])

  function valueChanged(e) {
    const addedItemsCpy = [...addedItems]
    addedItemsCpy[index][e.target.name] = e.target.value
    setAddedItems(addedItemsCpy)
  }

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
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Quantity Requested</Form.Label>
            <InputGroup className="mb-3">
              <Form.Control
                type="text"
                name="qty"
                readOnly
                value={addedItems[index].qtyRequested}
              />
              <Form.Control
                name="unit"
                type="text"
                readOnly
                value={addedItems[index].units[0]}
              />
            </InputGroup>
          </Form.Group>
        </div>
      </div>
      <div className="row">
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
        <div className="col">
          <div className="row">
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Unit Cost</Form.Label>
                <Form.Control
                  type="text"
                  name="cost"
                  value={addedItems[index].cost}
                  readOnly
                />
              </Form.Group>
            </div>
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Requested Total Cost</Form.Label>
                <Form.Control
                  type="text"
                  name="total"
                  readOnly
                  value={
                    addedItems[index].qtyRequested * addedItems[index].cost
                  }
                />
              </Form.Group>
            </div>
          </div>
        </div>
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
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Quantity Approved</Form.Label>
                <InputGroup className="mb-3">
                  <Form.Control
                    type="text"
                    name="qty"
                    readOnly
                    value={addedItems[index].qtyApproved}
                  />
                  <Form.Control
                    name="unit"
                    type="text"
                    readOnly
                    value={addedItems[index].units[0]}
                  />
                </InputGroup>
              </Form.Group>
            </div>
          </div>
          <div className="row">
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Quantity Purchased</Form.Label>
                <Form.Control
                  min="0"
                  type="number"
                  name="qtyPurchased"
                  value={addedItems[index].qtyPurchased}
                  onChange={(e) => {
                    if (e.target.value <= addedItems[index].qtyApproved)
                      valueChanged(e)
                  }}
                  required
                />
              </Form.Group>
            </div>
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Purchased Total Cost</Form.Label>
                <Form.Control
                  type="text"
                  name="total"
                  readOnly
                  value={
                    addedItems[index].qtyPurchased * addedItems[index].cost
                  }
                />
              </Form.Group>
            </div>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Request Remark</Form.Label>
            <Form.Control
              type="text"
              readOnly
              value={addedItems[index].requestRemark}
            />
          </Form.Group>
        </div>
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Approve Remark</Form.Label>
            <Form.Control
              type="text"
              readOnly
              value={addedItems[index].approveRemark}
            />
          </Form.Group>
        </div>
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Purchase Remark</Form.Label>
            <Form.Control
              type="text"
              name="purchaseRemark"
              value={addedItems[index].purchaseRemark}
              onChange={valueChanged}
            />
          </Form.Group>
        </div>
      </div>
    </div>
  )
}

export default PurchaseConfirmEquipment

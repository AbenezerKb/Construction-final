import React from "react"
import PropTypes from "prop-types"
import PurchaseConfirmMaterial from "./fragments/PurchaseConfirmMaterial"
import PurchaseConfirmEquipment from "./fragments/PurchaseConfirmEquipment"
import { Button, Form, Spinner } from "react-bootstrap"
import { confirmPurchase } from "../../../api/purchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function ConfirmPurchase({ addedItems, setAddedItems, purchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isConfirmLoading,
    isError: isConfirmError,
    error: confirmError,
    mutate: submitConfirmPurchase,
  } = useMutation(confirmPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isConfirmLoading) return

    const data = {
      purchaseId: purchase.purchaseId,
      purchaseItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyPurchased: Number(item.qtyPurchased),
        purchaseRemark: item.purchaseRemark,
      }

      if (item.type === ITEMTYPE.EQUIPMENT)
        tempItem.equipmentModelId = item.equipmentModelId

      data.purchaseItems.push(tempItem)
    }

    submitConfirmPurchase(data)
  }

  if (isConfirmError)
    return <ConnectionError status={confirmError?.response?.status} />

  return (
    <>
      <Form onSubmit={submit}>
        {addedItems.map((item, index) =>
          item.type == ITEMTYPE.MATERIAL ? (
            <PurchaseConfirmMaterial
              key={index}
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              index={index}
            />
          ) : (
            <PurchaseConfirmEquipment
              key={index}
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              index={index}
            />
          )
        )}

        <div className="d-grid">
          <Button className="btn-teal-dark" type="submit">
            {isConfirmLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Confirm Purchase
          </Button>
        </div>
      </Form>
    </>
  )
}

ConfirmPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  purchase: PropTypes.object.isRequired,
}

export default ConfirmPurchase

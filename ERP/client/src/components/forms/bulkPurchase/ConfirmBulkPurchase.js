import React from "react"
import PropTypes from "prop-types"
import BulkPurchaseConfirm from "./fragments/BulkPurchaseConfirm"
import { Button, Form, Spinner } from "react-bootstrap"
import { confirmBulkPurchase } from "../../../api/bulkPurchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function ConfirmBulkPurchase({ addedItems, setAddedItems, bulkPurchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isConfirmLoading,
    isError: isConfirmError,
    error: confirmError,
    mutate: submitConfirmBulkPurchase,
  } = useMutation(confirmBulkPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "bulkPurchase",
        bulkPurchase.bulkPurchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isConfirmLoading) return

    const data = {
      bulkPurchaseId: bulkPurchase.bulkPurchaseId,
      bulkPurchaseItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyPurchased: Number(item.qtyPurchased),
        purchaseRemark: item.purchaseRemark,
      }

      if (item.type === ITEMTYPE.EQUIPMENT)
        tempItem.equipmentModelId = item.equipmentModelId

      data.bulkPurchaseItems.push(tempItem)
    }

    submitConfirmBulkPurchase(data)
  }

  if (isConfirmError)
    return <ConnectionError status={confirmError?.response?.status} />

  return (
    <>
      <Form onSubmit={submit}>
        {addedItems.map((item, index) => (
          <BulkPurchaseConfirm
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        ))}

        <div className="d-grid">
          <Button className="btn-teal-dark" type="submit">
            {isConfirmLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Confirm BulkPurchase
          </Button>
        </div>
      </Form>
    </>
  )
}

ConfirmBulkPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  bulkPurchase: PropTypes.object.isRequired,
}

export default ConfirmBulkPurchase

import React from "react"
import PropTypes from "prop-types"
import PurchaseCheckMaterial from "./fragments/PurchaseCheckMaterial"
import PurchaseCheckEquipment from "./fragments/PurchaseCheckEquipment"
import { Button, Spinner, Form } from "react-bootstrap"
import { checkPurchase, queuePurchase } from "../../../api/purchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function CheckPurchase({ addedItems, setAddedItems, purchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isCheckLoading,
    isError: isCheckError,
    error: checkError,
    mutate: submitCheckPurchase,
  } = useMutation(checkPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  const {
    isLoading: isQueueLoading,
    isError: isQueueError,
    error: queueError,
    mutate: submitQueuePurchase,
  } = useMutation(queuePurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isCheckLoading || isQueueLoading) return
    const type = e.nativeEvent.submitter.name

    const data = {
      purchaseId: purchase.purchaseId,
    }

    if (type === "queue") submitQueuePurchase(data)
    else submitCheckPurchase(data)
  }

  if (isCheckError || isQueueError)
    return (
      <ConnectionError
        status={checkError?.response?.status && queueError?.response?.status}
      />
    )
  return (
    <Form onSubmit={submit}>
      {addedItems.map((item, index) =>
        item.type == ITEMTYPE.MATERIAL ? (
          <PurchaseCheckMaterial
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        ) : (
          <PurchaseCheckEquipment
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        )
      )}

      <div className="row">
        <div className="col-8 d-grid">
          <Button className="btn-teal-dark" type="submit" name="check">
            {isCheckLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Check
          </Button>
        </div>
        <div className="col-4 d-grid">
          <Button variant="warning" type="submit" name="queue">
            {isQueueLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Queue
          </Button>
        </div>
      </div>
    </Form>
  )
}

CheckPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  purchase: PropTypes.object.isRequired,
}

export default CheckPurchase

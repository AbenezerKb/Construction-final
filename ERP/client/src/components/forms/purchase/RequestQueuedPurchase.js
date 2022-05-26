import React from "react"
import PropTypes from "prop-types"
import PurchaseRequestQueuedMaterial from "./fragments/PurchaseRequestQueuedMaterial"
import PurchaseRequestQueuedEquipment from "./fragments/PurchaseRequestQueuedEquipment"
import { Button, Form, Spinner } from "react-bootstrap"
import { requestQueuedPurchase } from "../../../api/purchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function RequestQueuedPurchase({ addedItems, setAddedItems, purchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isRequestLoading,
    isError: isRequestError,
    error: requestError,
    mutate: submitRequestQueuedPurchase,
  } = useMutation(requestQueuedPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isRequestLoading) return

    const data = {
      purchaseId: purchase.purchaseId,
    }

    submitRequestQueuedPurchase(data)
  }

  if (isRequestError)
    return (
      <ConnectionError
        status={requestError?.response?.status && queueError?.response?.status}
      />
    )

  return (
    <>
      <Form onSubmit={submit}>
        {addedItems.map((item, index) =>
          item.type == ITEMTYPE.MATERIAL ? (
            <PurchaseRequestQueuedMaterial
              key={index}
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              index={index}
            />
          ) : (
            <PurchaseRequestQueuedEquipment
              key={index}
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              index={index}
            />
          )
        )}

        <div className="d-grid">
          <Button className="btn-teal-dark" type="submit">
            {isRequestLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Request Purchase
          </Button>
        </div>
      </Form>
    </>
  )
}

RequestQueuedPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  purchase: PropTypes.object.isRequired,
}

export default RequestQueuedPurchase

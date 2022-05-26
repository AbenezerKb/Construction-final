import React from "react"
import PropTypes from "prop-types"
import PurchaseApproveMaterial from "./fragments/PurchaseApproveMaterial"
import PurchaseApproveEquipment from "./fragments/PurchaseApproveEquipment"
import { Button, Spinner, Form } from "react-bootstrap"
import { approvePurchase, declinePurchase } from "../../../api/purchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function ApprovePurchase({ addedItems, setAddedItems, purchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isApproveLoading,
    isError: isApproveError,
    error: approveError,
    mutate: submitApprovePurchase,
  } = useMutation(approvePurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  const {
    isLoading: isDeclineLoading,
    isError: isDeclineError,
    error: declineError,
    mutate: submitDeclinePurchase,
  } = useMutation(declinePurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "purchase",
        purchase.purchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isApproveLoading || isDeclineLoading) return
    const type = e.nativeEvent.submitter.name

    const data = {
      purchaseId: purchase.purchaseId,
      purchaseItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyApproved: Number(item.qtyApproved),
        approveRemark: item.approveRemark,
      }

      if (item.type === ITEMTYPE.EQUIPMENT)
        tempItem.equipmentModelId = item.equipmentModelId

      data.purchaseItems.push(tempItem)
    }

    if (type === "decline") submitDeclinePurchase(data)
    else submitApprovePurchase(data)
  }

  if (isApproveError || isDeclineError)
    return (
      <ConnectionError
        status={
          approveError?.response?.status && declineError?.response?.status
        }
      />
    )

  return (
    <Form onSubmit={submit}>
      {addedItems.map((item, index) =>
        item.type == ITEMTYPE.MATERIAL ? (
          <PurchaseApproveMaterial
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        ) : (
          <PurchaseApproveEquipment
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        )
      )}

      <div className="row">
        <div className="col-8 d-grid">
          <Button className="btn-teal-dark" type="submit" name="approve">
            {isApproveLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Approve
          </Button>
        </div>
        <div className="col-4 d-grid">
          <Button variant="danger" type="submit" name="decline">
            {isDeclineLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Decline
          </Button>
        </div>
      </div>
    </Form>
  )
}

ApprovePurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  purchase: PropTypes.object.isRequired,
}

export default ApprovePurchase

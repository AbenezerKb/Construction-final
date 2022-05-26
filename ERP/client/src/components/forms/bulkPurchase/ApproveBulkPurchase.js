import React from "react"
import PropTypes from "prop-types"
import BulkPurchaseApprove from "./fragments/BulkPurchaseApprove"
import { Button, Spinner, Form } from "react-bootstrap"
import {
  approveBulkPurchase,
  declineBulkPurchase,
} from "../../../api/bulkPurchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function ApproveBulkPurchase({ addedItems, setAddedItems, bulkPurchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isApproveLoading,
    isError: isApproveError,
    error: approveError,
    mutate: submitApproveBulkPurchase,
  } = useMutation(approveBulkPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "bulkPurchase",
        bulkPurchase.bulkPurchaseId.toString(),
      ]),
  })

  const {
    isLoading: isDeclineLoading,
    isError: isDeclineError,
    error: declineError,
    mutate: submitDeclineBulkPurchase,
  } = useMutation(declineBulkPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "bulkPurchase",
        bulkPurchase.bulkPurchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isApproveLoading || isDeclineLoading) return
    const type = e.nativeEvent.submitter.name

    const data = {
      bulkPurchaseId: bulkPurchase.bulkPurchaseId,
      bulkPurchaseItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyApproved: Number(item.qtyApproved),
        approveRemark: item.approveRemark,
      }

      if (item.type === ITEMTYPE.EQUIPMENT)
        tempItem.equipmentModelId = item.equipmentModelId

      data.bulkPurchaseItems.push(tempItem)
    }

    if (type === "decline") submitDeclineBulkPurchase(data)
    else submitApproveBulkPurchase(data)
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
      {addedItems.map((item, index) => (
        <BulkPurchaseApprove
          key={index}
          addedItems={addedItems}
          setAddedItems={setAddedItems}
          index={index}
        />
      ))}

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

ApproveBulkPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  bulkPurchase: PropTypes.object.isRequired,
}

export default ApproveBulkPurchase

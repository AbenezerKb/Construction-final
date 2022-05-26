import React from "react"
import PropTypes from "prop-types"
import BulkPurchaseRequest from "./fragments/BulkPurchaseRequest"
import { Button, Form, Spinner } from "react-bootstrap"
import { requestBulkPurchase } from "../../../api/bulkPurchase"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function RequestBulkPurchase({ addedItems, setAddedItems, bulkPurchase }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isRequestLoading,
    isError: isRequestError,
    error: requestError,
    mutate: submitRequestBulkPurchase,
  } = useMutation(requestBulkPurchase, {
    onSuccess: (res) =>
      queryClient.invalidateQueries([
        "bulkPurchase",
        bulkPurchase.bulkPurchaseId.toString(),
      ]),
  })

  function submit(e) {
    e.preventDefault()
    if (isRequestLoading) return

    const data = {
      bulkPurchaseId: bulkPurchase.bulkPurchaseId,
    }

    submitRequestBulkPurchase(data)
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
        {addedItems.map((item, index) => (
          <BulkPurchaseRequest
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        ))}

        <div className="d-grid">
          <Button className="btn-teal-dark" type="submit">
            {isRequestLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Request BulkPurchase
          </Button>
        </div>
      </Form>
    </>
  )
}

RequestBulkPurchase.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  bulkPurchase: PropTypes.object.isRequired,
}

export default RequestBulkPurchase

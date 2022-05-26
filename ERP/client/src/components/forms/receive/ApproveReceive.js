import React from "react"
import PropTypes from "prop-types"
import ReceiveApproveItem from "./fragments/ReceiveApproveItem"
import { Button, Spinner, Form } from "react-bootstrap"
import { approveReceive } from "../../../api/receive"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"

function ApproveReceive({ addedItems, setAddedItems, receive }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isApproveLoading,
    isError: isApproveError,
    error: approveError,
    mutate: submitApproveReceive,
  } = useMutation(approveReceive, {
    onSuccess: (res) =>
      queryClient.invalidateQueries(["receive", receive.receiveId.toString()]),
  })

  function submit(e) {
    e.preventDefault()
    if (isApproveLoading) return

    const data = {
      receiveId: receive.receiveId,
    }

    submitApproveReceive(data)
  }

  if (isApproveError)
    return <ConnectionError status={approveError?.response?.status} />

  return (
    <Form onSubmit={submit}>
      {addedItems.map((item, index) => (
        <ReceiveApproveItem
          key={index}
          addedItems={addedItems}
          setAddedItems={setAddedItems}
          index={index}
        />
      ))}

      <div className="row">
        <div className="d-grid">
          <Button className="btn-teal-dark" type="submit" name="approve">
            {isApproveLoading && (
              <Spinner className="me-2" animation="grow" size="sm" />
            )}
            Approve
          </Button>
        </div>
      </div>
    </Form>
  )
}

ApproveReceive.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  receive: PropTypes.object.isRequired,
}

export default ApproveReceive

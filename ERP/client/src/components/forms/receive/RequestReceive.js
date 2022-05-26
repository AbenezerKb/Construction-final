import React from "react"
import PropTypes from "prop-types"
import ReceiveItem from "./fragments/ReceiveItem"
import { Button, Form, Spinner } from "react-bootstrap"
import { requestReceive } from "../../../api/receive"
import { useMutation, useQueryClient } from "react-query"
import ConnectionError from "../../fragments/ConnectionError"
import { ITEMTYPE } from "../../../Constants"

function RequestReceive({ addedItems, setAddedItems, receive }) {
  const queryClient = useQueryClient()

  const {
    isLoading: isRequestLoading,
    isError: isRequestError,
    error: receiveError,
    mutate: submitRequestReceive,
  } = useMutation(requestReceive, {
    onSuccess: (res) =>
      queryClient.invalidateQueries(["receive", receive.receiveId.toString()]),
  })

  function submit(e) {
    e.preventDefault()
    if (isRequestLoading) return

    const data = {
      receiveId: receive.receiveId,
      receiveItems: [],
    }

    for (let item of addedItems) {
      let tempItem = {
        itemId: Number(item.itemId),
        qtyReceived: Number(item.qtyReceived),
        receiveRemark: item.receiveRemark,
      }

      if (item.type === ITEMTYPE.EQUIPMENT)
        tempItem.equipmentModelId = item.equipmentModelId

      data.receiveItems.push(tempItem)
    }

    submitRequestReceive(data)
  }

  if (isRequestError)
    return <ConnectionError status={receiveError?.response?.status} />
  return (
    <>
      <Form onSubmit={submit}>
        {addedItems.map((item, index) => (
          <ReceiveItem
            key={index}
            addedItems={addedItems}
            setAddedItems={setAddedItems}
            index={index}
          />
        ))}

        <div className="row">
          <div className="d-grid">
            <Button className="btn-teal-dark" type="submit">
              {isRequestLoading && (
                <Spinner className="me-2" animation="grow" size="sm" />
              )}
              Receive
            </Button>
          </div>
        </div>
      </Form>
    </>
  )
}

RequestReceive.propTypes = {
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.func.isRequired,
  receive: PropTypes.object.isRequired,
}

export default RequestReceive

import { Alert, Stack, Typography } from "@mui/material";
import { useListTransactionsQuery } from "../services/backendApi";
import { TransactionItem } from "./TransactionItem";

export function TransactionList() {
  const { data: transactions, isFetching, isError, error } =
    useListTransactionsQuery();

  const normalizedErrorMessage = (() => {
    if (!error) return undefined;
    if ("status" in error) {
      const data = error.data as Record<string, unknown> | undefined;
      if (typeof data === "object" && data && "message" in data) {
        return String(data.message);
      }
      return `Request failed with status ${error.status}.`;
    }
    return error.message ?? "Something went wrong while loading transactions.";
  })();

  return (
    <div>
      <Typography variant="h5">Transaction history</Typography>

      {isFetching && !transactions ? (
        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          Loading transactions…
        </Typography>
      ) : null}

      {isError ? (
        <Alert severity="error" sx={{ mt: 2 }}>
          {normalizedErrorMessage ??
            "Unable to load transactions. Please try again."}
        </Alert>
      ) : null}

      <Stack spacing={2} aria-live="polite" sx={{ mt: 2 }}>
        {transactions?.length === 0 && !isFetching ? (
          <Typography variant="body2" color="text.secondary">
            No transactions yet.
          </Typography>
        ) : (
          transactions?.map((transaction, idx) => (
            <TransactionItem
              key={transaction.transaction_id}
              transaction={transaction}
              showCurrentBalance={idx === 0}
            />
          ))
        )}
      </Stack>
    </div>
  );
}
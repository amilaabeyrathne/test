export type Transaction = {
    transaction_id: string;
    account_id: string;
    amount: number;
    created_at: string;
  }
  
  export type CreateTransactionParams = {
      account_id: string;
      amount: number;
  }
  
  export type Account = {
      account_id: string;
      balance: number;
  }
  
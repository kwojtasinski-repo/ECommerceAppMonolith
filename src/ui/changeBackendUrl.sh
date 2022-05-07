# override ApiUrl
sed -i '/\(REACT_APP_BACKEND_URL=\).*/\1$API_URL/g' .env.production
cat .env.production
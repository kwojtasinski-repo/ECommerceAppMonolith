# override ApiUrl
sed -i 's,{BACKEND_URL},'"$API_URL"',g' .env.production
cat .env.production
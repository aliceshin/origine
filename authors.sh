java -jar svn-migration-scripts.jar \
authors https://ccview.bistel.co.kr:8080/svn/BISTel/branches/qa/ees/1.1.2/src/client/common \
> a1.txt

cat a1.txt | sort | uniq | sed -e s/mycompany.com/bistel.co.kr/g > authors.txt
 
rm -f a1.txt


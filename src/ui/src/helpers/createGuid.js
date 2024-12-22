export function createGuid(version) {  
   switch (version) {
      case 'N' :
         return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            let r, v;
            return ((r = Math.random() * 16 | 0), (v = c === 'x' ? r : ((r & 0x3) | 0x8)), v.toString(16));
         });
      default :
         return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {  
            let r, v;
            return ((r = Math.random() * 16 | 0), (v = c === 'x' ? r : ((r & 0x3) | 0x8)), v.toString(16));
         });
   }
}
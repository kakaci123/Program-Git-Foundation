package maxlike.maxlike.src.maxlike;

import java.util.*;





public class countevent {
	  private ArrayList eventlist = new ArrayList();

	  public countevent() {

	  }
	//�Nitem�[�ievent
	  public void addItemToEvent(item i){
	    if (!checkSameItem(i.getItemAttrName())){
	      eventlist.add(i);
	    }
	  }

	//check�b�P�@event���O�_���ۦP�ݩ�
	  private boolean checkSameItem(short attrname){
	    for(int i=0;i<eventlist.size();i++){
	      if (attrname == ((item)(eventlist.get(i))).getItemAttrName()){
	        return true;
	      }
	    }
	    return false;
	  }

	//���oitem
	  public item getItem(int i){
	    return (item)(eventlist.get(i));//�j���૬,�ݬO�_����L��k
	  }

	//���oitem�Ӽ�
	  public int getItemNum(){
	    return eventlist.size();
	  }
	//��ܲ�i��item
	  public void showitem(int i){
	    item tempitem = (item)(eventlist.get(i));
	    System.out.print("(" + tempitem.getItemAttrName() + ", " + tempitem.getItemAttrValue() + ")");
	  }

	}
